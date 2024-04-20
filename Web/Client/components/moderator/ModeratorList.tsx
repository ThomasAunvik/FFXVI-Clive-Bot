import axios from "axios";
import { useCallback, useEffect, useState } from "react";
import {
  Accordion,
  Button,
  ButtonGroup,
  Col,
  Collapse,
  Form,
  ListGroup,
  ListGroupItem,
} from "react-bootstrap";
import type { IModerator } from "../../lib/models/moderator/ModeratorModel";
import {
  ErrorModal,
  type ErrorModalInfo,
  getErrorInfo,
} from "../errors/ErrorHandler";

import {
  faPencil,
  faPlus,
  faSave,
  faTrash,
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import useIsMounted from "../misc/useIsMounted";
import { ModeratorListForm } from "./ModeratorListForm";
import { ModeratorSingleForm, toSentence } from "./ModeratorSingleForm";

export const ModeratorList = () => {
  const isMounted = useIsMounted();

  const [moderators, setModerators] = useState<IModerator[]>([]);

  const [openAddNew, setOpenAddNew] = useState(false);

  const [error, setError] = useState<ErrorModalInfo | null>(null);

  const fetchModerator = useCallback(async () => {
    try {
      const res = await axios.get("/api/moderator");
      if (res.status == 200) {
        const newModerators = res.data as IModerator[];
        if (isMounted()) {
          setModerators(newModerators);
        }
      }
    } catch (err: any) {
      setError(getErrorInfo(err));
    }
  }, [isMounted]);

  useEffect(() => {
    fetchModerator();
  }, [fetchModerator]);

  return (
    <div className="mb-4">
      <ButtonGroup className="mb-4">
        <Button
          onClick={() => {
            setOpenAddNew(true);
          }}
        >
          <FontAwesomeIcon icon={faPlus} width={20} />
        </Button>
      </ButtonGroup>
      <Collapse in={openAddNew}>
        <div>
          <ModeratorSingleForm
            onSuccess={(newMods) => {
              setOpenAddNew(false);
              setModerators(newMods);
            }}
            close={() => setOpenAddNew(false)}
          />
        </div>
      </Collapse>

      <Accordion>
        {moderators.map((s, i) => {
          return (
            <Accordion.Item eventKey={i.toString()} key={"moderator-" + s.id}>
              <Accordion.Header>
                {s.name} ({s.connectionSource})
              </Accordion.Header>
              <Accordion.Body>
                <ModeratorListForm
                  moderator={s}
                  onDelete={async () => {
                    try {
                      var res = await axios.delete("/api/moderator/" + s.id);
                      if (res.status == 200) {
                        setModerators(res.data as IModerator[]);
                      }
                    } catch (err: any) {
                      setError(getErrorInfo(err));
                    }
                  }}
                  onUpdate={(mods) => {
                    setModerators(mods);
                  }}
                />
              </Accordion.Body>
            </Accordion.Item>
          );
        })}
      </Accordion>
      {error == null ? null : (
        <ErrorModal error={error} onHide={() => setError(null)} />
      )}
    </div>
  );
};
