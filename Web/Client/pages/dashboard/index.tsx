import DashboardNavBar from "@/components/DashboardNavBar";
import axios from "axios";
import Head from "next/head";
import { useState } from "react";
import { Button, Col, Container, Form, Modal, Row } from "react-bootstrap";

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
        <Container className="mt-3">
          <h1>Welcome to the FFXVI Clive Discord Bot Dashboard</h1>
        </Container>
      </main>
    </>
  );
};

export default DashboardPage;
