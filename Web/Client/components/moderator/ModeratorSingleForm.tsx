import { ErrorModal, ErrorModalInfo, getErrorInfo } from "@/components/errors/ErrorHandler";
import axios from "axios"
import { Formik } from "formik"
import { useState } from "react";
import { Button, ButtonGroup, Col, Form, ListGroup, ListGroupItem, Row } from "react-bootstrap"
import { IModerator } from "../models/moderator/ModeratorModel";

export interface IModeratorSingleFormProps {
    onSuccess: (moderators: IModerator[]) => void;
    close: () => void;
}

export const ModeratorSingleForm = (props: IModeratorSingleFormProps) => {
    const { close, onSuccess } = props;

    const [error, setError] = useState<ErrorModalInfo | null>(null);

    const onSumbit = async (values: IModerator) => {
        var res = await axios.post("/api/moderator", values);
        if(res.status == 200) {
            onSuccess(res.data as IModerator[]);
        }
    }

    return <Formik<IModerator>
        initialValues={{
            name: "",
            connectionSource: "Discord",
            connectionId: "",
            permissions: {
                manageModerators: false,
                allPermissions: true,
                manageCharacterInfo: false,
                manageCharacterNotes: false,
                manageCharacters: false,
                manageSkillInfo: false,
                manageSkills: false,
                manageSkillTranslations: false,
            }
        } as IModerator}
        onSubmit={async (values, action) => {
            try {
                await onSumbit(values);
            } catch(err: any) {
                setError(getErrorInfo(err));
            }
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
		setFieldValue,
    }) => (
        <Form 
            onSubmit={handleSubmit}
            onReset={handleReset}
        >
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
                {["Discord"].map(s => 
                    <option value={s} key={"connectionsource-" + s}>
                        {s}
                    </option>
                )}
            </Form.Control>
            </Form.Group>
            <Form.Group>
                <Form.Label>Connection Id</Form.Label>
                <Form.Control 
                    name="connectionId" 
                    type="text"
                    onChange={handleChange}
                    onBlur={handleBlur}
                    value={values.connectionId}
                />
            </Form.Group>
            <Form.Group className="mt-4">
                <ListGroup>
                    {Object.keys(values.permissions).map((key, i) => {
                        const allowed = Object.values(values.permissions)[i];
                        const objectKey = "permissions." + key;
                        return <ListGroupItem key={"permission-" + key}>
                            <Form.Check 
                                name={objectKey}
                                label={toSentence(key)}
                                type="checkbox"
                                onChange={handleChange}
                                onBlur={handleBlur}
                                checked={allowed ?? false}
                                size={20}
                            />
                        </ListGroupItem>
                 })}
                </ListGroup>
            </Form.Group>

            <Form.Group className="mt-2 mb-4">
                <ButtonGroup>
                    <Button 
                        variant="primary"
                        type="submit"
                        disabled={!dirty || isSubmitting}
                    >
                        Submit
                    </Button>
                    <Button 
                        variant="warning"
                        type="reset"
                        disabled={!dirty || isSubmitting}
                    >
                        Reset
                    </Button>
                    <Button 
                        variant="secondary"
                        onClick={close}
                    >
                        Cancel
                    </Button>
                </ButtonGroup>
            </Form.Group>
            
            {error == null ? null :
                <ErrorModal error={error} onHide={() => setError(null)} />
            }
        </Form>
    )}
    </Formik>
}

export const toSentence = (header: string) => {
    let newHeader: string[] = [];
    let chars = Array.from(header);
    chars.forEach((char: string) => {
        if (char.toUpperCase() && char != char.toLowerCase()){
            newHeader.push( ' ');
            newHeader.push( char);
        
        } else {
            newHeader.push(char);
        }
        newHeader[0] = newHeader[0].toUpperCase();
        
    })
    var result = newHeader.join('');
    return result;
}