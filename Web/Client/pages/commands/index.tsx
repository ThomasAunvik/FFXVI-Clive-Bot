import Aux from "@/components/Auxillary";
import { commandCategories } from "@/components/Commands";
import MainPageNavBar from "@/components/MainPageNavBar";
import Head from "next/head";
import type { MouseEvent } from "react";
import { Container, Table } from "react-bootstrap";

import styles from "styles/Commands.module.css";

export default function CommandsPage() {
  // biome-ignore lint/suspicious/noExplicitAny: <explanation>
  const collapseClick = (e: any) => {
    const hiddenElement = e.currentTarget.nextSibling;
	if(!hiddenElement) return;
    hiddenElement.className.indexOf("collapse show") > -1
      ? hiddenElement.classList.remove("show")
      : hiddenElement.classList.add("show");
  };

  return (
    <>
      <Head>
        <title>Clive Bot - Commands</title>
        <meta
          name="description"
          content="A List of Commands for the Clive Bot"
        />
      </Head>
      <main>
        <MainPageNavBar currentPath="/commands" />
        <Container style={{ marginTop: "20px" }}>
          <h1>Commands</h1>
          {commandCategories.map((category) => (
            <Aux key={`category-${category.name}`}>
              <h2>{category.name}</h2>
              <Table>
                <thead>
                  <tr>
                    <th>Slash Command</th>
                    <th>Description</th>
                    <th>Permissions</th>
                  </tr>
                </thead>
                <tbody>
                  {category.commands.map((command) => (
                    <Aux key={`command-${command.command}`}>
                      {/* biome-ignore lint/a11y/useKeyWithClickEvents: <explanation> */}
					<tr onClick={collapseClick} className={styles.tablerow}>
                        <td>/{command.command}</td>
                        <td>{command.description}</td>
                        <td>
                          {command.permissions.map((p) => p.name).join(", ")}
                        </td>
                      </tr>
                      <tr
                        className="collapse"
                        key={`command-collapse-${command.command}`}
                      >
                        <td colSpan={3}>
                          <Table>
                            <thead>
                              <tr>
                                <th>Arguments</th>
                                <th>Description</th>
                                <th>Parameter Type</th>
                                <th>Is Nullable</th>
                              </tr>
                            </thead>
                            <tbody>
                              {command.arguments.map((arg) => (
                                <tr
                                  key={
                                    `command-${command.command}-arg-${arg.parameter}`
                                  }
                                >
                                  <td>{arg.parameter}</td>
                                  <td>{arg.description}</td>
                                  <td>{arg.parameterType}</td>
                                  <td>{arg.nullable ? "Yes" : "No"}</td>
                                </tr>
                              ))}
                            </tbody>
                          </Table>
                        </td>
                      </tr>
                    </Aux>
                  ))}
                </tbody>
              </Table>
            </Aux>
          ))}
        </Container>
      </main>
    </>
  );
}
