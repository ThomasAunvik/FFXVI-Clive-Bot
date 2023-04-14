import { AxiosError } from "axios";
import { Button, Modal } from "react-bootstrap";

export const getErrorInfo = (error: any): ErrorModalInfo => {
  if (error instanceof AxiosError) {
    var errorMessage = error.response?.data?.message;
    if (errorMessage == null) {
      errorMessage = error.message;
    }

    return {
      statusCode: error.response?.status ?? 0,
      statusMessage: error.response?.statusText ?? error.message,
      message: errorMessage,
    };
  }

  return {
    statusCode: 0,
    statusMessage: "Unknown Error",
    message: error.toString(),
  };
};

export interface ErrorModalInfo {
  statusCode: number;
  statusMessage: string;
  message?: string;
}

export interface ErrorModalProps {
  error: ErrorModalInfo;
  onHide: () => void;
}

export const ErrorModal = (props: ErrorModalProps) => {
  const { error, onHide } = props;

  return (
    <Modal onHide={onHide} show={true}>
      <Modal.Header closeButton>
        <Modal.Title>
          {error.statusCode} Error: {error.statusMessage}
        </Modal.Title>
      </Modal.Header>

      <Modal.Body>
        <p>{error.message ?? "Unknown Error"}</p>
      </Modal.Body>

      <Modal.Footer>
        <Button variant="primary" onClick={onHide}>
          Ok
        </Button>
      </Modal.Footer>
    </Modal>
  );
};
