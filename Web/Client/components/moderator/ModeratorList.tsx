import axios from "axios";
import { useCallback, useEffect, useState } from "react";
import { Accordion, Button, ButtonGroup, ListGroup, Col, Collapse } from "react-bootstrap";
import { ErrorModal, ErrorModalInfo, getErrorInfo } from "../errors/ErrorHandler";
import { IModerator } from "../models/moderator/ModeratorModel";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faPlus, faTrash } from '@fortawesome/free-solid-svg-icons'
import useIsMounted from "../misc/useIsMounted";
import { ModeratorSingleForm } from "./ModeratorSingleForm";

export const ModeratorList = () => {    
    const isMounted = useIsMounted();

    const [moderators, setModerators] = useState<IModerator[]>([]);

    const [openAddNew, setOpenAddNew] = useState(false);

    const [error, setError] = useState<ErrorModalInfo | null>(null);

    const fetchModerator = useCallback(async () => {
        try {
            const res = await axios.get("/api/moderator");
            if(res.status == 200) {
                const newModerators = res.data as IModerator[];
                if(isMounted()) {
                    setModerators(newModerators);
                }
            }
        }catch(err: any) {
            setError(getErrorInfo(err));
        }
    }, [isMounted]);

    useEffect(() => {
        fetchModerator();
    }, [fetchModerator]);

    return <div>
            <ButtonGroup className="mb-4">
                <Button
                    onClick={() => {
                        setOpenAddNew(true);
                    }}
                >
                    <FontAwesomeIcon icon={faPlus} width={20}/>
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
                return (<Accordion.Item eventKey={i.toString()} key={"moderator-" + s.toString()}>
                <Accordion.Header>{s.name} ({s.connectionSource})</Accordion.Header>
                <Accordion.Body>
                    <div>
                        <Button
                            variant="danger"
                            className="mb-3"
                            onClick={async () => {
                                try {
                                    var res = await axios.delete("/api/moderator/" + s.id);
                                    if(res.status == 200) {
                                        setModerators(res.data as IModerator[]);
                                    }
                                } catch(err: any) {
                                    setError(getErrorInfo(err));
                                } 
                            }}
                        >
                            <FontAwesomeIcon icon={faTrash} width={20} />
                        </Button>
                        <ListGroup variant="flush">
                            {Object.values(s).map(s => {
                                return <ListGroup.Item key={"skill-" + s}>
                                    <span style={{marginLeft: "1em"}}>{s}</span>
                                </ListGroup.Item >
                            })}
                        </ListGroup>
                    </div>
                </Accordion.Body>
            </Accordion.Item>)
            })}
        </Accordion>
        {error == null ? null :
            <ErrorModal error={error} onHide={() => setError(null)} />
        }
    </div>
}