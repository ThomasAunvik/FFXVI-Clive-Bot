import DashboardNavBar from "@/components/DashboardNavBar";
import useIsMounted from "@/components/misc/useIsMounted";
import { SkillSummonList } from "@/components/skills/SkillSummonList";
import { faAdd } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Head from "next/head";
import { useEffect, useState } from "react";
import { Button, Col, Container, Row } from "react-bootstrap";

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
        <Container style={{ marginTop: "2em" }}>
          <h2>Summons</h2>
          <Button href={"/dashboard/skills/new"} className="mb-3">
            <FontAwesomeIcon icon={faAdd} width={20} />
          </Button>
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
