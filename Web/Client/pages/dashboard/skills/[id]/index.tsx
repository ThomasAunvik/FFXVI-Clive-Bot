import DashboardNavBar from "@/components/DashboardNavBar";
import Head from "next/head";

const DashboardSkillPage = () => {
  return (
    <>
      <Head>
        <title>Clive Bot - Dashboard</title>
        <meta name="description" content="Skill Dashboard" />
      </Head>
      <main>
        <DashboardNavBar currentPath="/dashboard/skills" />
      </main>
    </>
  );
};

export default DashboardSkillPage;
