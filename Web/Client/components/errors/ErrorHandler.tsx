import { Button, Modal } from "react-bootstrap"

export interface ErrorModalInfo {
	statusCode: number;
	statusMessage: string;
	message?: string;
}

export interface ErrorModalProps {
	error: ErrorModalInfo,
	onHide: () => void,
}

export const ErrorModal = (props: ErrorModalProps) => {
	const { error, onHide } = props;

	return (<Modal onHide={onHide} show={true}>
        <Modal.Header closeButton>
          <Modal.Title>{error.statusCode} Error: {error.statusMessage}</Modal.Title>
        </Modal.Header>
		
        <Modal.Body>
          <p>{error.message ?? "Unknown Error"}</p>
        </Modal.Body>

        <Modal.Footer>
          <Button variant="primary" onClick={onHide}>Ok</Button>
        </Modal.Footer>
	</Modal>)
}