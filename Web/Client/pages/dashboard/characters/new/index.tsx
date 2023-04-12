import { CharacterForm } from "@/components/characters/CharacterForm";
import DashboardNavBar from "@/components/DashboardNavBar";
import axios from "axios";
import Head from "next/head";
import { Button, Col, Container, Spinner } from "react-bootstrap";

const DashboardNewCharacterPage = () => {
  return (
    <>
      <Head>
        <title>Clive Bot - New Character Dashboard</title>
        <meta name="description" content="Character Dashboard" />
      </Head>
      <main>
        <DashboardNavBar currentPath="/dashboard/characters" />
        <Container className="mb-4">
          <Button variant="link" href="/dashboard/characters">Return to Characters</Button>
          <Col md={4}>
            <CharacterForm />
          </Col>
        </Container>
      </main>
    </>
  );
};

export default DashboardNewCharacterPage;
