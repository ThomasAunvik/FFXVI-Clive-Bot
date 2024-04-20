import DashboardNavBar from "@/components/DashboardNavBar";
import { ModeratorList } from "@/components/moderator/ModeratorList";
import Head from "next/head";
import { Button, Col, Container, Row } from "react-bootstrap";

const DashboardModeratorPage = () => {
  return (
    <>
      <Head>
        <title>Clive Bot - Dashboard</title>
        <meta name="description" content="Skill Dashboard" />
      </Head>
      <main>
        <DashboardNavBar currentPath="/dashboard/settings/moderator" />
        <Container style={{ marginTop: "2em" }}>
          <h2>Moderators</h2>
          <Col md={3}>
            <Row ml={3}>
              <ModeratorList />
            </Row>
          </Col>
        </Container>
      </main>
    </>
  );
};

export default DashboardModeratorPage;
