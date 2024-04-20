import { faPencil, faSave, faTrash } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import axios from "axios";
import { Formik } from "formik";
import { isUndefined } from "lodash";
import { useState } from "react";
import { Button, ButtonGroup, Col, Form, Row, Spinner } from "react-bootstrap";
import type { ISkillLanguage } from "../../lib/models/skill/SkillLanguageModel";
import {
  ErrorModal,
  type ErrorModalInfo,
  getErrorInfo,
} from "../errors/ErrorHandler";

export interface ISkillLanguageFormProps {
  skillId: string;
  language?: ISkillLanguage;
  onDelete?: () => void;
  onUpdate: (languages: ISkillLanguage[]) => void;
}

export const SkillLanguageForm = (props: ISkillLanguageFormProps) => {
  const { skillId, language, onDelete, onUpdate } = props;

  const [initialValues, setInitialValues] = useState<ISkillLanguage>(
    language ??
      ({
        id: 0,
        locale: "",
        name: "New Skill Name",
        description: "New Skill Description",
      } as ISkillLanguage),
  );

  const [error, setError] = useState<ErrorModalInfo | null>(null);

  return (
    <Formik<ISkillLanguage>
      initialValues={initialValues}
      enableReinitialize
      onSubmit={async (values, action) => {
        try {
          var res = await axios.post(
            `/api/skill/${skillId}/languages/${values.locale}`,
            values,
          );
          if (res.status == 200) {
            var data = res.data as ISkillLanguage[];
            onUpdate(data);
            if (isUndefined(language)) {
              setInitialValues({
                id: 0,
                locale: "",
                name: "New Skill Name",
                description: "New Skill Description",
              } as ISkillLanguage);
            }
          }
        } catch (err: any) {
          setError(getErrorInfo(err));
        }
        action.setSubmitting(false);
      }}
    >
      {({
        values,
        dirty,
        handleChange,
        handleBlur,
        handleSubmit,
        handleReset,
        isSubmitting,
      }) => (
        <Form onSubmit={handleSubmit} onReset={handleReset}>
          <Row className="mb-3">
            {onDelete == null ? null : (
              <Col>
                <Button variant="danger" onClick={onDelete}>
                  <FontAwesomeIcon icon={faTrash} width={20} />
                </Button>
              </Col>
            )}

            <Col style={{ flex: 0 }}>
              <Button
                variant="success"
                type="submit"
                disabled={!dirty || isSubmitting}
              >
                {isSubmitting ? (
                  <div>
                    <Spinner
                      as="span"
                      animation="border"
                      size="sm"
                      role="status"
                      aria-hidden="true"
                    />
                    <span className="visually-hidden">Loading...</span>
                  </div>
                ) : (
                  <FontAwesomeIcon icon={faSave} width={20} />
                )}
              </Button>
            </Col>
          </Row>

          <Form.Group className="mb-2">
            <Form.Label>Locale</Form.Label>
            <Form.Control
              name="locale"
              as="select"
              onChange={handleChange}
              onBlur={handleBlur}
              value={values.locale}
              disabled={language != null}
            >
              {["en", "no", "jp", "de", "fr"].map((l) => (
                <option value={l} key={"locale-" + l}>
                  {l}
                </option>
              ))}
            </Form.Control>
          </Form.Group>
          <Form.Group className="mb-2">
            <Form.Label>Skill Name</Form.Label>
            <Form.Control
              name="name"
              defaultValue={values.name}
              onChange={handleChange}
              onBlur={handleBlur}
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Skill Description</Form.Label>
            <Form.Control
              name="description"
              as="textarea"
              rows={3}
              onChange={handleChange}
              onBlur={handleBlur}
              value={values.description}
            />
          </Form.Group>

          {error == null ? null : (
            <ErrorModal error={error} onHide={() => setError(null)} />
          )}
        </Form>
      )}
    </Formik>
  );
};
