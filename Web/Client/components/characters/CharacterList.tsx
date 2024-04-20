import axios from "axios";
import Image from "next/image";
import { useEffect, useState } from "react";
import {
  Accordion,
  Button,
  Card,
  Col,
  ListGroup,
  Row,
  Spinner,
} from "react-bootstrap";
import type { ICharacter } from "../../lib/models/characters/CharacterModel";
import {
  ISkill,
  SkillSummon,
  summonList,
} from "../../lib/models/skill/SkillModel";
import { replaceCDN } from "../constants";
import {
  ErrorModal,
  type ErrorModalInfo,
  getErrorInfo,
} from "../errors/ErrorHandler";

export const CharacterList = () => {
  const [characters, setCharacters] = useState<ICharacter[] | null>(null);

  const [error, setError] = useState<ErrorModalInfo | null>(null);

  const fetchCharacters = async () => {
    try {
      const res = await axios.get("/api/character");
      if (res.status == 200) {
        const newCharacters = res.data as ICharacter[];
        setCharacters(newCharacters);
      }
    } catch (err: any) {
      setError(getErrorInfo(err));
    }
  };

  useEffect(() => {
    fetchCharacters();
  }, []);

  return (
    <div>
      {characters === null ? (
        <Spinner animation="border" role="status">
          <span className="visually-hidden">Loading...</span>
        </Spinner>
      ) : (
        <Row>
          {characters.map((s, i) => {
            const variant = s.variants?.find((v) => v.defaultVariant);

            return (
              <Col style={{ marginBottom: "2em" }} key={"character-" + s.id}>
                <Card style={{ width: "90vw", maxWidth: "18rem" }}>
                  <Card.Img
                    variant="top"
                    src="/static/images/features/char-bg.webp"
                  />
                  <Card.ImgOverlay>
                    <Card.Img
                      src={
                        variant?.previewImageUrl
                          ? replaceCDN(variant?.previewImageUrl)
                          : "https://cdn.discordapp.com/attachments/1075203421696700488/1075205728505167883/cliveRosfield_art_pc.png"
                      }
                    />
                  </Card.ImgOverlay>
                  <Card.Body
                    style={{ backgroundColor: "var(--bs-card-bg)", zIndex: 1 }}
                  >
                    <Card.Title>{s.name}</Card.Title>
                    <Card.Text>
                      {s.variants?.find((v) => v.defaultVariant)?.description ??
                        "No Description"}
                    </Card.Text>
                    <Button
                      href={"/dashboard/characters/" + s.id}
                      variant="primary"
                    >
                      Edit
                    </Button>
                  </Card.Body>
                </Card>
              </Col>
            );
          })}
        </Row>
      )}
      {error == null ? null : (
        <ErrorModal error={error} onHide={() => setError(null)} />
      )}
    </div>
  );
};
