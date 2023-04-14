import DashboardNavBar from "@/components/DashboardNavBar";
import {
  ErrorModal,
  ErrorModalInfo,
  getErrorInfo,
} from "@/components/errors/ErrorHandler";
import { ISkill } from "@/components/models/skill/SkillModel";
import { SkillForm } from "@/components/skills/SkillForm";
import axios from "axios";
import Head from "next/head";
import { useRouter } from "next/router";
import { useCallback, useEffect, useRef, useState } from "react";
import { Button, Col, Container, Spinner } from "react-bootstrap";

const DashboardNewSkillPage = () => {
  return (
    <>
      <Head>
        <title>Clive Bot - New Skill Dashboard</title>
        <meta name="description" content="Skill Dashboard" />
      </Head>
      <main>
        <DashboardNavBar currentPath="/dashboard/skills" />
        <Container className="mb-4">
          <Button variant="link" href="/dashboard/skills">
            Return to Skills
          </Button>
          <h1>New Skill</h1>
          <Col md={4}>
            <SkillForm />
          </Col>
        </Container>
      </main>
    </>
  );
};

export default DashboardNewSkillPage;
