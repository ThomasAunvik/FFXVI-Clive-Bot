import DashboardNavBar from "@/components/DashboardNavBar";
import useIsMounted from "@/components/misc/useIsMounted";
import { SkillSummonList } from "@/components/skills/SkillSummonList";
import Head from "next/head";
import { useEffect, useState } from "react";
import { Col, Container, Row } from "react-bootstrap";

const DashboardSkillListPage = () => {
  const isMounted = useIsMounted();

  const [skills, setSkills] = useState([]);

  useEffect(() => {}, [isMounted]);

  return (
    <>
      <Head>
        <title>Clive Bot - Dashboard</title>
        <meta name="description" content="Skill Dashboard" />
      </Head>
      <main>
        <DashboardNavBar currentPath="/dashboard/skills" />
        <Container style={{ marginTop: "2em"}}>
          <h2>Summons</h2>
          <Col md={3}>
            <Row ml={3}>
              <SkillSummonList />
            </Row>
          </Col>
        </Container>
      </main>
    </>
  );
};

export default DashboardSkillListPage;
