import axios, { AxiosError, type AxiosProgressEvent, CancelToken } from "axios";
import { Formik, type FormikHelpers } from "formik";
import _, { isNull } from "lodash";
import { useRef, useState } from "react";
import {
  Button,
  ButtonGroup,
  Col,
  Collapse,
  Form,
  Row,
  Spinner,
} from "react-bootstrap";
import {
  type ISkill,
  SkillCategory,
  SkillSummon,
  skillCategoryList,
  summonList,
} from "../../lib/models/skill/SkillModel";
import { replaceCDN } from "../constants";
import {
  ErrorModal,
  type ErrorModalInfo,
  getErrorInfo,
} from "../errors/ErrorHandler";
import { UploadProgress } from "../upload/UploadProgress";

export interface ISkillFormProps {
  skill?: ISkill;
}

interface FormikProps {
  iconFile: File | null;
  previewFile: File | null;
}

type FormikFormProps = ISkill & FormikProps;

export const SkillForm = (props: ISkillFormProps) => {
  const { skill } = props;

  const [initialSkill, setInitialSkill] = useState<ISkill>(
    skill ?? {
      id: 0,
      name: "",
      description: "",
      localized: [],
      category: SkillCategory.None,
      summon: SkillSummon.Ifrit,
      ratingPhysical: 0,
      ratingMagical: 0,
      costBuy: 0,
      costUpgrade: 0,
      costMaster: 0,
    },
  );

  const [error, setError] = useState<ErrorModalInfo | null>(null);

  const cancelUploads = useRef(new AbortController());
  const [iconFileProgress, setIconFileProgress] =
    useState<AxiosProgressEvent | null>(null);
  const [previewFileProgress, setPreviewFileProgress] =
    useState<AxiosProgressEvent | null>(null);

  const submitForm = async (
    values: ISkill & FormikProps,
    actions: FormikHelpers<FormikFormProps>,
  ) => {
    const { iconFile, previewFile, ...newSkill } = values;

    var skillId = skill?.id ?? null;
    if (skill == null) {
      const res = await axios.post("/api/skill/", newSkill);
      if (res.status != 200) {
        setError({
          statusCode: res.status,
          statusMessage: res.statusText,
          message: res.data.message,
        });
        return null;
      }

      var newInitialSkill = res.data as ISkill;
      skillId = newInitialSkill.id;
      setInitialSkill(newInitialSkill);
    } else {
      if (!_.isEqual(newSkill, initialSkill)) {
        const res = await axios.put("/api/skill/" + skill.id, newSkill);
        if (res.status != 200) {
          setError({
            statusCode: res.status,
            statusMessage: res.statusText,
            message: res.data.message,
          });
          return null;
        }

        var newInitialSkill = res.data as ISkill;
        setInitialSkill(newInitialSkill);
      }
    }

    if (iconFile != null && !isNull(skillId)) {
      const iconForm = new FormData();
      iconForm.append("iconFile", iconFile);
      const res = await axios.postForm(
        "/api/skill/" + skillId + "/images/icon",
        iconForm,
        {
          onDownloadProgress: (prog) => {
            setIconFileProgress({ ...prog });
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

    if (previewFile != null && !isNull(skillId)) {
      const previewForm = new FormData();
      previewForm.append("previewFile", previewFile);
      const res = await axios.postForm(
        "/api/skill/" + skillId + "/images/preview",
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

    console.log(skill);
    if ((isNull(skill) || skill === undefined) && !isNull(skillId)) {
      console.log("test");
      document.location.replace("/dashboard/skills/" + skillId);
    }

    return;
  };

  const formik = (
    <Formik<FormikFormProps>
      initialValues={{ ...initialSkill } as FormikFormProps}
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
            <Form.Label>Description</Form.Label>
            <Form.Control
              name="description"
              as={"textarea"}
              rows={3}
              value={values.description}
              onChange={handleChange}
              onBlur={handleBlur}
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Category</Form.Label>
            <Form.Control
              as="select"
              name="category"
              defaultValue={SkillCategory[values.category]}
              onChange={(event) => {
                var value = event.currentTarget.value as unknown;
                setFieldValue(
                  "category",
                  SkillCategory[value as SkillCategory],
                );
              }}
              onBlur={handleBlur}
            >
              {skillCategoryList.map((s) => (
                <option value={s} key={"category-" + s}>
                  {s}
                </option>
              ))}
            </Form.Control>
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Summon</Form.Label>
            <Form.Control
              as="select"
              name="summon"
              defaultValue={SkillSummon[values.summon]}
              onChange={(event) => {
                var value = event.currentTarget.value as unknown;
                setFieldValue("summon", SkillSummon[value as SkillSummon]);
              }}
              onBlur={handleBlur}
            >
              {summonList.map((s) => (
                <option value={s} key={"summon-" + s}>
                  {s}
                </option>
              ))}
            </Form.Control>
          </Form.Group>

          <Form.Group className="mb-3">
            <Form.Label>Physical Rating</Form.Label>
            <Row>
              <Col xs="4">
                <Form.Control
                  name="ratingPhysical"
                  type="number"
                  value={values.ratingPhysical}
                  onChange={handleChange}
                  onBlur={handleBlur}
                  min={0}
                  max={10}
                  step={1}
                />
              </Col>
              <Col xs="8">
                <Form.Range
                  name="ratingPhysical"
                  value={values.ratingPhysical}
                  onChange={handleChange}
                  onBlur={handleBlur}
                  min={0}
                  max={10}
                  step={1}
                />
              </Col>
            </Row>
          </Form.Group>

          <Form.Group className="mb-3">
            <Form.Label>Magical Rating</Form.Label>
            <Row>
              <Col xs="4">
                <Form.Control
                  name="ratingMagical"
                  type="number"
                  value={values.ratingMagical}
                  onChange={handleChange}
                  onBlur={handleBlur}
                  min={0}
                  max={10}
                  step={1}
                />
              </Col>
              <Col xs="8">
                <Form.Range
                  name="ratingMagical"
                  value={values.ratingMagical}
                  onChange={handleChange}
                  onBlur={handleBlur}
                  min={0}
                  max={10}
                  step={1}
                />
              </Col>
            </Row>
          </Form.Group>

          <Form.Group className="mb-3">
            <Form.Label>Cost Buy</Form.Label>
            <Form.Control
              name="costBuy"
              type="number"
              value={values.costBuy}
              onChange={handleChange}
              onBlur={handleBlur}
            />
          </Form.Group>

          <Form.Group className="mb-3">
            <Form.Label>Cost Upgrade</Form.Label>
            <Form.Control
              name="costUpgrade"
              type="number"
              value={values.costUpgrade}
              onChange={handleChange}
              onBlur={handleBlur}
            />
          </Form.Group>

          <Form.Group className="mb-3">
            <Form.Label>Cost Master</Form.Label>
            <Form.Control
              name="costMaster"
              type="number"
              value={values.costMaster}
              onChange={handleChange}
              onBlur={handleBlur}
            />
          </Form.Group>

          <Form.Group className="mb-3">
            <Form.Label>Icon</Form.Label>
            <Form.Control
              name="iconFile"
              type="file"
              accept="image/*"
              className="mb-2"
              onChange={(event) => {
                setFieldValue(
                  "iconFile",
                  (event.currentTarget as any).files[0],
                );
              }}
              onBlur={handleBlur}
            />
            <Collapse in={iconFileProgress != null}>
              <div>
                <UploadProgress progress={iconFileProgress} />
              </div>
            </Collapse>
            <Form.Control
              name="iconUrl"
              value={values.iconUrl ?? ""}
              onChange={handleChange}
              onBlur={handleBlur}
            />
            <Button
              variant="link"
              disabled={values.iconUrl == null}
              href={replaceCDN(values.iconUrl ?? "")}
              target="_blank"
            >
              Preview Image
            </Button>
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

              {(values.iconFile != null || values.previewFile != null) &&
              isSubmitting ? (
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

  return formik;
};
