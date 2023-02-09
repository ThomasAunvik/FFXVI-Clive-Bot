import DashboardNavBar from "@/components/DashboardNavBar";
import axios from "axios";
import Head from "next/head";
import { useState } from "react";
import { Button, Form, Modal } from "react-bootstrap";

const DashboardPage = () => {
  const [show, setShow] = useState(false);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);
  
  return (
    <>
      <Head>
        <title>Clive Bot - Dashboard</title>
        <meta name="description" content="Admin Dashboard" />
      </Head>
      <main>
        <DashboardNavBar currentPath="/dashboard" />
        <Button variant="primary" onClick={handleShow}>
          Launch demo modal
        </Button>
        <Modal show={show} onHide={handleClose}>
          <Form action="/api/skill/1/images" method="dialog" onSubmit={async (form) => {
            const formData = new FormData(form.currentTarget);
            var res = await axios.postForm(
              "/api/skill/1/images", 
              formData, 
              { 
                onUploadProgress: (progress) => {

                }
              }
            );
          }}>
            <Modal.Header closeButton>
              <Modal.Title>Modal heading</Modal.Title>
            </Modal.Header>
            <Modal.Body>
              <Form.Group className="mb-3" controlId="formBasicEmail">
                <Form.Label>Email address</Form.Label>
                <Form.Control name="file-input" type="file" />
                <Form.Text className="text-muted">
                  {"We'll never share your email with anyone else."}
                </Form.Text>
              </Form.Group>
              <Button type="submit">Submit</Button>
            </Modal.Body>
            <Modal.Footer>
              <Button variant="secondary" onClick={handleClose}>
                Close
              </Button>
              <Button variant="primary" onClick={handleClose}>
                Save Changes
              </Button>
            </Modal.Footer>
          </Form>
        </Modal>
      </main>
    </>
  );
};

export default DashboardPage;
