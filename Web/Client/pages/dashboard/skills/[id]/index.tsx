import DashboardNavBar from "@/components/DashboardNavBar";
import {
  ErrorModal,
  ErrorModalInfo,
  getErrorInfo,
} from "@/components/errors/ErrorHandler";
import { ISkillLanguage } from "@/components/models/skill/SkillLanguageModel";
import { ISkill } from "@/components/models/skill/SkillModel";
import { SkillForm } from "@/components/skills/SkillForm";
import { SkillLanguageList } from "@/components/skills/SkillLanguagesList";
import axios from "axios";
import Head from "next/head";
import { useRouter } from "next/router";
import { useCallback, useEffect, useRef, useState } from "react";
import { Button, Col, Container, Row, Spinner } from "react-bootstrap";

const DashboardSkillPage = () => {
  const router = useRouter();
  const { id } = router.query;

  const firstTick = useRef(false);
  const [skill, setSkill] = useState<ISkill | null>(null);
  const [skillLanguages, setSkillLanguages] = useState<ISkillLanguage[] | null>(
    null
  );

  const [error, setError] = useState<ErrorModalInfo | null>(null);

  const fetchSkill = useCallback(async (skillId: string) => {
    try {
      const res = await axios.get("/api/skill/" + skillId);
      if (res.status != 200) return;

      setSkill(res.data as ISkill);
    } catch (err: any) {
      setError(getErrorInfo(err));
    }
  }, []);

  const fetchSkillLanguages = useCallback(async (skillId: string) => {
    try {
      const res = await axios.get("/api/skill/" + skillId + "/languages");
      if (res.status != 200) return;

      setSkillLanguages(res.data as ISkillLanguage[]);
    } catch (err: any) {
      setError(getErrorInfo(err));
    }
  }, []);

  useEffect(() => {
    if (!id) return;
    if (firstTick.current) return;
    firstTick.current = true;

    fetchSkill(id.toString());
    fetchSkillLanguages(id.toString());
  }, [fetchSkill, fetchSkillLanguages, id]);

  return (
    <>
      <Head>
        <title>Clive Bot - Skill Dashboard</title>
        <meta name="description" content="Skill Dashboard" />
      </Head>
      <main>
        <DashboardNavBar currentPath="/dashboard/skills" />
        <Container className="mb-4">
          <Button variant="link" href="/dashboard/skills">
            Return to Skills
          </Button>

          <h1>{skill?.name == null ? "Loading..." : skill.name}</h1>
          <Row>
            <Col md={4}>
              {skill == null ? (
                <Spinner animation="border" role="status">
                  <span className="visually-hidden">Loading...</span>
                </Spinner>
              ) : (
                <SkillForm skill={skill} />
              )}
            </Col>
            <Col md={6}>
              {!id ? (
                <Spinner animation="border" role="status">
                  <span className="visually-hidden">Loading...</span>
                </Spinner>
              ) : (
                <SkillLanguageList skillId={id as string} />
              )}
            </Col>
          </Row>

          {error == null ? null : (
            <ErrorModal error={error} onHide={() => setError(null)} />
          )}
        </Container>
      </main>
    </>
  );
};

export default DashboardSkillPage;
