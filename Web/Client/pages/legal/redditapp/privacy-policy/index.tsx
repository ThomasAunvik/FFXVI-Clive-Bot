import Head from "next/head";
import { policy } from "@/components/legal/redditapp/privacy-policy";
import MainPageNavBar from "@/components/MainPageNavBar";
import React from "react";
import { Container } from "react-bootstrap";

const DashboardPage = () => {
  const [firstLine, ...rest] = policy.split("\n");

  return (
    <>
      <Head>
        <title>FFXVI Clive Reddit App - Privacy Policy</title>
        <meta
          name="description"
          content="FFXVI Clive Reddit App - Privacy Policy"
        />
      </Head>
      <main>
        <MainPageNavBar currentPath="/privacy-policy" />
        <Container>
          <p>
            {firstLine}
            {rest.map((line, i) => (
              // React.Fragment doesn’t create a wrapper element in the DOM.
              // If you don’t care about that, you can use div instead
			  // biome-ignore lint/suspicious/noArrayIndexKey: <explanation>
              <React.Fragment key={`line-${i}`}>
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
