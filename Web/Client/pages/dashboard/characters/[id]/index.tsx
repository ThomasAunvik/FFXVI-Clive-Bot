import { CharacterForm } from "@/components/characters/CharacterForm";
import DashboardNavBar from "@/components/DashboardNavBar";
import axios from "axios";
import Head from "next/head";
import { Button, Col, Container, Spinner } from "react-bootstrap";
import { ICharacter } from "@/components/models/characters/CharacterModel";
import { useRouter } from "next/router";
import { useCallback, useEffect, useRef, useState } from "react";
import { ErrorModal, ErrorModalInfo, getErrorInfo } from "@/components/errors/ErrorHandler";

const DashboardCharacterPage = () => {
    const router = useRouter();
    const { id } = router.query;

    const firstTick = useRef(false);
    const [character, setCharacter] = useState<ICharacter | null>(null);

    const [error, setError] = useState<ErrorModalInfo | null>(null);

    const fetchCharacer = useCallback(async (characterId: string) => {
        try {
            const res = await axios.get("/api/character/" + characterId);
            if(res.status != 200) return;

            setCharacter(res.data as ICharacter);
        } catch(err: any) {
            setError(getErrorInfo(err));
        }
    }, []);


    useEffect(() => {
        if(!id) return;
        if(firstTick.current) return;
        firstTick.current = true;

        fetchCharacer(id.toString());
    }, [fetchCharacer, id]);

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
                </Container>

                {error == null ? null :
				    <ErrorModal error={error} onHide={() => setError(null)} />
			     }
            </main>
            </>
            );
};

export default DashboardCharacterPage;
