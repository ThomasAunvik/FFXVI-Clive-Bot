import DashboardNavBar from "@/components/DashboardNavBar";
import Head from "next/head";
import { Button, Container } from "react-bootstrap";

const DashboardSettingsPage = () => {
  return (
    <>
      <Head>
        <title>Clive Bot - Dashboard</title>
        <meta name="description" content="Skill Dashboard" />
      </Head>
      <main>
        <DashboardNavBar currentPath="/dashboard/settings" />
        <Container style={{ marginTop: "2em" }}>
          <h2>Settings</h2>
          <Button variant="link" href="/dashboard/settings/moderator">
            Moderators
          </Button>
        </Container>
      </main>
    </>
  );
};

export default DashboardSettingsPage;
