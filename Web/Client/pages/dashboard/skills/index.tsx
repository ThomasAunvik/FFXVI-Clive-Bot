import DashboardNavBar from "@/components/DashboardNavBar";
import useIsMounted from "@/components/misc/useIsMounted";
import Head from "next/head";
import { useEffect, useState } from "react";

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
      </main>
    </>
  );
};

export default DashboardSkillListPage;
