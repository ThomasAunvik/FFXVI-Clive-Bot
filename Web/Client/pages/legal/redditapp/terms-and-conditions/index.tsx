import Head from "next/head";
import { policy } from "@/components/legal/redditapp/toc";
import MainPageNavBar from "@/components/MainPageNavBar";
import React from "react";
import { Container } from "react-bootstrap";

const DashboardPage = () => {
  const [firstLine, ...rest] = policy.split("\n");

  return (
    <>
      <Head>
        <title>FFXVI Clive Reddit App - Terms of Conditions</title>
        <meta
          name="description"
          content="FFXVI Clive Reddit App - Terms and Conditions"
        />
      </Head>
      <main>
        <MainPageNavBar currentPath="/terms-and-conditions" />
        <Container>
          <p>
            {firstLine}
            {rest.map((line, i) => (
              // React.Fragment doesn’t create a wrapper element in the DOM.
              // If you don’t care about that, you can use div instead
              <React.Fragment key={"line-" + i}>
                <br />
                {line}
              </React.Fragment>
            ))}
          </p>
        </Container>
      </main>
    </>
  );
};

export default DashboardPage;
