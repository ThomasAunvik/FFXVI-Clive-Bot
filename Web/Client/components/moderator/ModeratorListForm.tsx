import {
  faPencil,
  faSave,
  faSpinner,
  faTrash,
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import axios from "axios";
import { Formik } from "formik";
import { useState } from "react";
import {
  Button,
  ButtonGroup,
  Collapse,
  Form,
  ListGroup,
  ListGroupItem,
  Spinner,
} from "react-bootstrap";
import type { IModerator } from "../../lib/models/moderator/ModeratorModel";
import {
  ErrorModal,
  type ErrorModalInfo,
  getErrorInfo,
} from "../errors/ErrorHandler";
import { toSentence } from "./ModeratorSingleForm";

export interface IModeratorListFormProps {
  moderator: IModerator;
  onDelete: () => void;
  onUpdate: (moderators: IModerator[]) => void;
}

export const ModeratorListForm = (props: IModeratorListFormProps) => {
  const { moderator, onDelete, onUpdate } = props;

  const [collapse, setCollapse] = useState(false);
  const [error, setError] = useState<ErrorModalInfo | null>(null);

  return (
    <Formik<IModerator>
      initialValues={moderator}
      enableReinitialize
      onSubmit={async (values, action) => {
        try {
          var res = await axios.put("/api/moderator/" + moderator.id, values);
          if (res.status == 200) {
            var data = res.data as IModerator[];
            onUpdate(data);
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
          <ButtonGroup className="mb-3">
            <Button variant="danger" onClick={onDelete}>
              <FontAwesomeIcon icon={faTrash} width={20} />
            </Button>
            <Button variant="secondary" onClick={() => setCollapse(!collapse)}>
              <FontAwesomeIcon
                icon={faPencil}
                width={20}
                style={{ display: "block" }}
              />
            </Button>
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
          </ButtonGroup>

          <Collapse in={collapse}>
            <div>
              <Form.Group>
                <Form.Label>Name</Form.Label>
                <Form.Control
                  name="name"
                  type="text"
                  onChange={handleChange}
                  onBlur={handleBlur}
                  value={values.name}
                />
              </Form.Group>
              <Form.Group>
                <Form.Label>Connection Source</Form.Label>
                <Form.Control
                  as="select"
                  name="connectionSource"
                  defaultValue={values.connectionSource}
                  onChange={handleChange}
                  onBlur={handleBlur}
                >
                  {["Discord"].map((s) => (
                    <option value={s} key={"connectionsource-" + s}>
                      {s}
                    </option>
                  ))}
                </Form.Control>
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Connection Id</Form.Label>
                <Form.Control
                  name="connectionId"
                  type="text"
                  onChange={handleChange}
                  onBlur={handleBlur}
                  value={values.connectionId}
                />
              </Form.Group>
            </div>
          </Collapse>

          <ListGroup variant="flush">
            {Object.keys(values.permissions).map((key) => {
              if (key == "id" || key == "moderator") return null;
              const allowed = (values.permissions as any)[key] as boolean;
              const objectKey = "permissions." + key;
              return (
                <ListGroupItem key={"permission-" + key}>
                  <Form.Check
                    name={objectKey}
                    label={toSentence(key)}
                    type="checkbox"
                    checked={allowed ?? false}
                    onChange={handleChange}
                    onBlur={handleBlur}
                    size={20}
                  />
                </ListGroupItem>
              );
            })}
          </ListGroup>
          {error == null ? null : (
            <ErrorModal error={error} onHide={() => setError(null)} />
          )}
        </Form>
      )}
    </Formik>
  );
};
