import DashboardNavBar from "@/components/DashboardNavBar";
import { ISkill } from "@/components/models/SkillModel";
import { SkillForm } from "@/components/skills/SkillForm";
import axios from "axios";
import Head from "next/head";
import { useRouter } from "next/router";
import { useCallback, useEffect, useRef, useState } from "react";
import { Button, Col, Container, Spinner } from "react-bootstrap";

const DashboardSkillPage = () => {
  const router = useRouter();
  const { id } = router.query;

  const firstTick = useRef(false);
  const [skill, setSkill] = useState<ISkill | null>(null);

  const fetchSkill = useCallback(async (skillId: string) => {
    const res = await axios.get("/api/skill/" + skillId);
    if(res.status != 200) return;

    setSkill(res.data as ISkill);
  }, []);

  useEffect(() => {
    if(!id) return;
    if(firstTick.current) return;
    firstTick.current = true;

    fetchSkill(id.toString());
  }, [fetchSkill, id]);

  return (
    <>
      <Head>
        <title>Clive Bot - Skill Dashboard</title>
        <meta name="description" content="Skill Dashboard" />
      </Head>
      <main>
        <DashboardNavBar currentPath="/dashboard/skills" />
        <Container className="mb-4">
          <Button variant="link" href="/dashboard/skills">Return to Skills</Button>

          <h1>{skill?.name == null ? "Loading..." : skill.name}</h1>
          <Col md={4}>
          { skill == null ?
            <Spinner animation="border" role="status">
              <span className="visually-hidden">Loading...</span>
            </Spinner> :
            <SkillForm skill={skill} />
          }
          </Col>
        </Container>
      </main>
    </>
  );
};

export default DashboardSkillPage;
