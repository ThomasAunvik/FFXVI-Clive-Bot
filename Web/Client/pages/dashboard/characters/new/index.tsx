import DashboardNavBar from "@/components/DashboardNavBar";
import { ErrorModal, ErrorModalInfo, getErrorInfo } from "@/components/errors/ErrorHandler";
import { ISkill } from "@/components/models/skill/SkillModel";
import { SkillForm } from "@/components/skills/SkillForm";
import axios from "axios";
import Head from "next/head";
import { useRouter } from "next/router";
import { useCallback, useEffect, useRef, useState } from "react";
import { Button, Col, Container, Spinner } from "react-bootstrap";

const DashboardNewCharacterPage = () => {
  return (
    <>
      <Head>
        <title>Clive Bot - New Character Dashboard</title>
        <meta name="description" content="Skill Dashboard" />
      </Head>
      <main>
        <DashboardNavBar currentPath="/dashboard/characters" />
        <Container className="mb-4">
          <Button variant="link" href="/dashboard/characters">Return to Characters</Button>
          <h1>New Character</h1>
          <Col md={4}>
            <CharacterForm />
          </Col>
        </Container>
      </main>
    </>
  );
};

export default DashboardNewCharacterPage;
