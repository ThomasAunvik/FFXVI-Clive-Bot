import axios, { AxiosProgressEvent } from "axios";
import { useState } from "react";
import { Button, Form, Modal } from "react-bootstrap";

export interface IUploadModalProps {
    show: boolean;
    handleClose: () => void;
}

export const UploadModal = (props: IUploadModalProps) => {
    const { show, handleClose } = props;
    
    const [uploadProgress, setUploadProgress] = useState<AxiosProgressEvent | null>(null);

    return (
    <Modal show={show} onHide={handleClose}>
      <Form action="/api/skill/1/images" method="dialog" onSubmit={async (form) => {
        const formData = new FormData(form.currentTarget);
        var res = await axios.postForm(
          "/api/skill/1/images", 
          formData, 
          { 
            onUploadProgress: (progress) => {
              setUploadProgress({...progress});
            }
          }
        );
      }}>
        <Modal.Header closeButton>
          <Modal.Title>Upload Image</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form.Group className="mb-3" controlId="formBasicEmail">
            <Form.Label>Email address</Form.Label>
            <Form.Control name="fileinput" type="file" />
            <Form.Text className="text-muted">
              {"We'll never share your email with anyone else."}
            </Form.>
          </Form.Group>
        </Modal.Body>
        <Modal.Footer>
          <Button type="submit">Submit</Button>
        </Modal.Footer>
      </Form>
    </Modal>
  );
}