import { CharacterForm } from "@/components/characters/CharacterForm";
import DashboardNavBar from "@/components/DashboardNavBar";
import axios from "axios";
import Head from "next/head";
import { Button, Col, Container, Row, Spinner } from "react-bootstrap";
import type { ICharacter } from "@/components/models/characters/CharacterModel";
import { useRouter } from "next/router";
import { useCallback, useEffect, useRef, useState } from "react";
import { ErrorModal, type ErrorModalInfo, getErrorInfo } from "@/components/errors/ErrorHandler";
import { ICharacterVariant } from "@/components/models/characters/CharacterVariant";
import { CharacterVariantList } from "@/components/characters/CharacterVariantList";

const DashboardCharacterPage = () => {
    const router = useRouter();
    const { id } = router.query;

    const firstTick = useRef(false);
    const [character, setCharacter] = useState<ICharacter | null>(null);
    const [error, setError] = useState<ErrorModalInfo | null>(null);

    const fetchCharacter = useCallback(async (characterId: string) => {
        try {
            const res = await axios.get(`/api/character/${characterId}`);
            if(res.status !== 200) return;

			const character = res.data as ICharacter;
            setCharacter(character);
        } catch(err) {
            setError(getErrorInfo(err));
        }
    }, []);

    useEffect(() => {
        if(!id) return;
        if(firstTick.current) return;
        firstTick.current = true;

        fetchCharacter(id.toString());
    }, [fetchCharacter, id]);

    return (
            <>
            <Head>
                <title>Clive Bot - Character Dashboard</title>
                <meta name="description" content="Character Dashboard" />
            </Head>
            <main>
                <DashboardNavBar currentPath="/dashboard/characters" />
                <Container className="mb-4">
                    <Button variant="link" href="/dashboard/characters">Return to Characters</Button>
                    <Row>
					<Col md={4}>

                        { character == null ?
        					<Spinner animation="border" role="status">
                                <span className="visually-hidden">Loading...</span>
                            </Spinner> :
        					<CharacterForm
                                character={character}
                            />
        				}
                    </Col>
					<Col md={4}>
					{ character == null ?
        					<Spinner animation="border" role="status">
                                <span className="visually-hidden">Loading...</span>
                            </Spinner> :
        					<CharacterVariantList
                                character={character}
                            />
        				}
					</Col>
					</Row>
                </Container>

                {error == null ? null :
				    <ErrorModal error={error} onHide={() => setError(null)} />
			     }
            </main>
            </>
            );
};

export default DashboardCharacterPage;
