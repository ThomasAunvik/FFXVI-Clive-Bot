import axios, { type AxiosProgressEvent } from "axios";
import { Formik, type FormikHelpers } from "formik";
import _, { isNull } from "lodash";
import { useRef, useState } from "react";
import { Button, ButtonGroup, Collapse, Form, Spinner } from "react-bootstrap";
import type { ICharacter } from "../../lib/models/characters/CharacterModel";
import type { ICharacterVariant } from "../../lib/models/characters/CharacterVariant";
import { replaceCDN } from "../constants";
import {
  ErrorModal,
  type ErrorModalInfo,
  getErrorInfo,
} from "../errors/ErrorHandler";
import { UploadProgress } from "../upload/UploadProgress";

interface CharacterVariantFormProps {
  character: ICharacter;
  variant?: ICharacterVariant;
  onSubmit: (variant: ICharacterVariant) => void;
  onCancel?: () => void;
}

interface FormikProps {
  previewFile: File | null;
}

type FormikFormProps = ICharacterVariant & FormikProps;

export const CharacterVariantForm = (props: CharacterVariantFormProps) => {
  const { variant, character, onSubmit, onCancel } = props;

  const [error, setError] = useState<ErrorModalInfo | null>(null);

  const cancelUploads = useRef(new AbortController());
  const [previewFileProgress, setPreviewFileProgress] =
    useState<AxiosProgressEvent | null>(null);

  const submitForm = async (
    values: ICharacterVariant & FormikProps,
    actions: FormikHelpers<FormikFormProps>,
  ) => {
    const { previewFile, ...newVariant } = values;

    let variantId = variant?.id ?? null;
    if (variant == null) {
      const res = await axios.post(
        `/api/character/${character.id}/variant`,
        newVariant,
      );
      if (res.status != 200) {
        setError({
          statusCode: res.status,
          statusMessage: res.statusText,
          message: res.data.message,
        });
        return null;
      }

      const newVariantData = res.data as ICharacterVariant;
      variantId = newVariantData.id;
      onSubmit(newVariantData);
    } else {
      if (!_.isEqual(newVariant, variant)) {
        const res = await axios.put(
          `/api/character/${character.id}/variant/${variant.id}`,
          newVariant,
        );
        if (res.status != 200) {
          setError({
            statusCode: res.status,
            statusMessage: res.statusText,
            message: res.data.message,
          });
          return null;
        }

        const newVariantData = res.data as ICharacterVariant;
        onSubmit(newVariantData);
      }
    }

    if (previewFile != null && !isNull(variantId)) {
      const previewForm = new FormData();
      previewForm.append("previewFile", previewFile);
      const res = await axios.postForm(
        `/api/character/${character.id}/variant/${variantId}/images/preview`,
        previewForm,
        {
          onDownloadProgress: (prog) => {
            setPreviewFileProgress({ ...prog });
          },
          signal: cancelUploads.current.signal,
        },
      );

      if (res.status != 200) {
        setError({
          statusCode: res.status,
          statusMessage: res.statusText,
          message: res.data.message,
        });
        return;
      }
    }

    return;
  };

  const formik = (
    <Formik<FormikFormProps>
      initialValues={{ ...variant } as FormikFormProps}
      enableReinitialize
      onSubmit={async (values, actions) => {
        try {
          await submitForm(values, actions);
        } catch (err: any) {
          setError(getErrorInfo(err));
        }

        actions.setSubmitting(false);
      }}
    >
      {({
        values,
        dirty,
        handleChange,
        handleBlur,
        handleSubmit,
        isSubmitting,
        setFieldValue,
      }) => (
        <Form onSubmit={handleSubmit}>
          <Form.Group className="mb-3">
            <Form.Label>Description</Form.Label>
            <Form.Control
              name="description"
              as="textarea"
              rows={4}
              onChange={handleChange}
              onBlur={handleBlur}
              value={values.description}
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Default Variant</Form.Label>
            <Form.Check
              name="defaultVariant"
              type="checkbox"
              onChange={handleChange}
              onBlur={handleBlur}
              checked={values.defaultVariant}
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Age</Form.Label>
            <Form.Control
              name="age"
              type="number"
              onChange={handleChange}
              onBlur={handleBlur}
              value={values.age}
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>From Year</Form.Label>
            <Form.Control
              name="fromYear"
              type="number"
              onChange={handleChange}
              onBlur={handleBlur}
              value={values.fromYear}
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>To Year</Form.Label>
            <Form.Control
              name="toYear"
              type="number"
              onChange={handleChange}
              onBlur={handleBlur}
              value={values.toYear}
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Preview Image</Form.Label>
            <Form.Control
              name="previewFile"
              type="file"
              accept="image/*"
              className="mb-2"
              onChange={(event) => {
                setFieldValue(
                  "previewFile",
                  (event.currentTarget as any).files[0],
                );
              }}
              onBlur={handleBlur}
            />
            <Collapse in={previewFileProgress != null}>
              <div>
                <UploadProgress progress={previewFileProgress} />
              </div>
            </Collapse>
            <Form.Control
              name="previewImageUrl"
              value={values.previewImageUrl ?? ""}
              onChange={handleChange}
              onBlur={handleBlur}
            />
            <Button
              variant="link"
              disabled={values.previewImageUrl == null}
              href={replaceCDN(values.previewImageUrl ?? "")}
              target="_blank"
            >
              Preview Image
            </Button>
          </Form.Group>

          <Form.Group>
            <ButtonGroup>
              <Button
                variant="primary"
                type="submit"
                disabled={!dirty || isSubmitting}
              >
                Submit
              </Button>

              {!variant && (
                <Button variant="secondary" onClick={onCancel}>
                  Cancel
                </Button>
              )}

              {values.previewFile != null && isSubmitting ? (
                <Button
                  variant="secondary"
                  onClick={() => {
                    cancelUploads.current.abort();
                  }}
                >
                  Cancel Upload
                </Button>
              ) : null}

              {!isSubmitting ? null : (
                <Button variant="secondary" disabled>
                  <Spinner
                    as="span"
                    animation="border"
                    size="sm"
                    role="status"
                    aria-hidden="true"
                  />
                  <span className="visually-hidden">Loading...</span>
                </Button>
              )}
            </ButtonGroup>
          </Form.Group>

          {error == null ? null : (
            <ErrorModal error={error} onHide={() => setError(null)} />
          )}
        </Form>
      )}
    </Formik>
  );

  return <div>{formik}</div>;
};
