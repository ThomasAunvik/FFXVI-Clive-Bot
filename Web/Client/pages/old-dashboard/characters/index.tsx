import { CharacterList } from "@/components/characters/CharacterList";
import DashboardNavBar from "@/components/DashboardNavBar";
import { faAdd } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Head from "next/head";
import { Button, Col, Container, Row } from "react-bootstrap";

const DashboardCharacterListPage = () => {
  return (
    <>
      <Head>
        <title>Clive Bot - Dashboard</title>
        <meta name="description" content="Character Dashboard" />
      </Head>
      <main>
        <DashboardNavBar currentPath="/dashboard/characters" />
        <Container style={{ marginTop: "2em" }}>
          <h2>Characters</h2>
          <Button href={"/dashboard/characters/new"} className="mb-3">
            <FontAwesomeIcon icon={faAdd} width={20} />
          </Button>
          <CharacterList />
        </Container>
      </main>
    </>
  );
};

export default DashboardCharacterListPage;
