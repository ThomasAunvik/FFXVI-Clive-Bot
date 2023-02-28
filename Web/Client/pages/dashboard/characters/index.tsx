import DashboardNavBar from "@/components/DashboardNavBar";
import { SkillSummonList } from "@/components/skills/SkillSummonList";
import { faAdd } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Head from "next/head";
import { Button, Col, Container, Row } from "react-bootstrap";

const DashboardCharacterListPage = () => {
  return (
    <>
		<Head>
			<title>Clive Bot - Dashboard</title>
			<meta name="description" content="Skill Dashboard" />
		</Head>
		<main>
			<DashboardNavBar currentPath="/dashboard/skills" />
			<Container style={{ marginTop: "2em"}}>
				<h2>Characters</h2>
				<Button
					href={"/dashboard/skills/new"}
					className="mb-3"
				>
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

export default DashboardCharacterListPage;
