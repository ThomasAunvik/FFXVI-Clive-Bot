import axios, { AxiosError, AxiosProgressEvent, CancelToken } from "axios";
import { Formik, FormikHelpers } from "formik";
import _, { isNull } from "lodash";
import { useRef, useState } from "react";
import { Button, ButtonGroup, Collapse, Form, Spinner } from "react-bootstrap";
import { replaceCDN } from "../constants";
import {
  ErrorModal,
  ErrorModalInfo,
  getErrorInfo,
} from "../errors/ErrorHandler";
import { ICharacter } from "../models/characters/CharacterModel";
import { ISkill } from "../models/skill/SkillModel";
import { UploadProgress } from "../upload/UploadProgress";

export interface ICharacterFormProps {
  character?: ICharacter;
}

interface FormikProps {
  previewFile: File | null;
}

type FormikFormProps = ISkill & FormikProps;

export const CharacterForm = (props: ICharacterFormProps) => {
  const { character } = props;

  const [initialCharacter, setInitialCharacter] = useState<ICharacter>(
    character ?? {
      id: 0,
      name: "",
    }
  );

  const [error, setError] = useState<ErrorModalInfo | null>(null);

  const cancelUploads = useRef(new AbortController());
  const [previewFileProgress, setPreviewFileProgress] =
    useState<AxiosProgressEvent | null>(null);

  const submitForm = async (
    values: ICharacter & FormikProps,
    actions: FormikHelpers<FormikFormProps>
  ) => {
    const { previewFile, ...newCharacter } = values;

    let characterId = character?.id ?? null;
    if (character == null) {
      const res = await axios.post("/api/character/", newCharacter);
      if (res.status != 200) {
        setError({
          statusCode: res.status,
          statusMessage: res.statusText,
          message: res.data.message,
        });
        return null;
      }

      let newInitialCharacter = res.data as ICharacter;
      characterId = newInitialCharacter.id;
      setInitialCharacter(newInitialCharacter);
    } else {
      if (!_.isEqual(newCharacter, initialCharacter)) {
        const res = await axios.put(
          "/api/character/" + character.id,
          newCharacter
        );
        if (res.status != 200) {
          setError({
            statusCode: res.status,
            statusMessage: res.statusText,
            message: res.data.message,
          });
          return null;
        }

        let newInitialCharacter = res.data as ICharacter;
        setInitialCharacter(newInitialCharacter);
      }
    }

    if (previewFile != null && !isNull(characterId)) {
      const previewForm = new FormData();
      previewForm.append("previewFile", previewFile);
      const res = await axios.postForm(
        "/api/character/" + characterId + "/images/preview",
        previewForm,
        {
          onDownloadProgress: (prog) => {
            setPreviewFileProgress({ ...prog });
          },
          signal: cancelUploads.current.signal,
        }
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

    if (
      (isNull(character) || character === undefined) &&
      !isNull(characterId)
    ) {
      document.location.replace("/dashboard/characters/" + characterId);
    }

    return;
  };

  const formik = (
    <Formik<FormikFormProps>
      initialValues={{ ...initialCharacter } as FormikFormProps}
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
            <Form.Label>Name</Form.Label>
            <Form.Control
              name="name"
              type="text"
              onChange={handleChange}
              onBlur={handleBlur}
              value={values.name}
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
                  (event.currentTarget as any).files[0]
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

  return (
    <div>
      <h1>{character?.name ?? "New Character"}</h1>
      {formik}
    </div>
  );
};
