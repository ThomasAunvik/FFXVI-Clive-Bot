import MainPageNavBar from '@/components/MainPageNavBar';
import axios from 'axios';
import Head from 'next/head'
import Image from 'next/image';
import { useEffect, useMemo, useState } from 'react';
import { Button, Col, Container, Nav, Navbar, NavDropdown, Row } from 'react-bootstrap';

import styles from 'styles/Home.module.css';

export default function Home() {
  return (
    <>
      <Head>
        <title>Clive Bot</title>
        <meta name="description" content="Clive Bot, the Final Fantasy XVI Bot for Discord" />
      </Head>
      <main>
        <div className={styles.content}>
          <MainPageNavBar
            currentPath='/'
          />
          <Container className={styles.container}>
              <Container className={styles.child}>
                <p className={styles.title}>
                  <span className={styles.titleff}>FFXVI</span> Clive Bot</p>
                <br/>
                <Button variant="primary" onClick={() => {
                  window.open(
                    "https://discord.com/oauth2/authorize?client_id=1070508781013848144&scope=bot&permissions=0",
                    "_blank"
                  )
                }}>
                  <span>
                    Add Bot to Discord{' '}
                    <Image alt="discord-icon" src="/images/discord/discord-mark-white.svg" width={20} height={20} className={"pull-right "}/>
                  </span>
                </Button>
                <br></br>
                <Button style={{marginTop: "30px"}} variant="primary" onClick={() => {
                  window.open(
                    "https://discord.gg/y34bsEg",
                    "_blank"
                  )
                }}>
                  <span>
                    Join Discord Server {' '}
                  </span>
                </Button>
              </Container>
          </Container>
        </div>
        <div className={styles.features}>
          <h1>Features</h1>

          <h2>Skill Preview</h2>
          <Image alt="Skill Preview of Lunge attack" src="/images/features/command_skill_preview.webp" width={523} height={516} className={styles.imagecontain} />
        </div>
      </main>
    </>
  )
}
