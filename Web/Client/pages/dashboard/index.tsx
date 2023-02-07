import DashboardNavBar from "@/components/DashboardNavBar";
import Head from "next/head";

const DashboardPage = () => {
  return (
    <>
      <Head>
        <title>Clive Bot - Dashboard</title>
        <meta name="description" content="Admin Dashboard" />
      </Head>
      <main>
        <DashboardNavBar currentPath="/dashboard" />
      </main>
    </>
  );
};

export default DashboardPage;
