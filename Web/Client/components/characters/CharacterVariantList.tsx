import {
  Accordion,
  Button,
  Card,
  Col,
  Collapse,
  Container,
  Row,
} from "react-bootstrap";
import { ICharacter } from "../models/characters/CharacterModel";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faAdd } from "@fortawesome/free-solid-svg-icons";
import { useState } from "react";
import { CharacterVariantForm } from "./CharacterVariantForm";

export interface ICharacterVariantListProps {
  character: ICharacter;
}

export const CharacterVariantList = (props: ICharacterVariantListProps) => {
  const { character } = props;

  const [variants, setVariants] = useState(character.variants ?? []);
  const [openNew, setOpenNew] = useState(false);

  const updateVariant = () => {};

  const newVariant = () => {};

  return (
    <div>
      <h2>
        Variants{" "}
        <Button onClick={() => setOpenNew(true)}>
          <FontAwesomeIcon icon={faAdd} width={16} />
        </Button>
      </h2>
      <Collapse in={openNew}>
        <div>
          <Card>
            <Card.Body>
              <CharacterVariantForm
                onSubmit={newVariant}
                character={character}
                onCancel={() => setOpenNew(false)}
              />
            </Card.Body>
          </Card>
        </div>
      </Collapse>

      <Accordion>
        {variants.map((v) => {
          return (
            <Accordion.Item
              key={`variant-${v.id}`}
              eventKey={`variant-${v.id}`}
            >
              <Accordion.Header>
                <Container>
                  <Row>
                    <Col sx={3}>Age: {v.age}</Col>
                    <Col>
                      Year {v.fromYear} - {v.toYear}
                    </Col>
                  </Row>
                </Container>
              </Accordion.Header>
              <Accordion.Body>
                <CharacterVariantForm
                  variant={v}
                  onSubmit={updateVariant}
                  character={character}
                />
              </Accordion.Body>
            </Accordion.Item>
          );
        })}
      </Accordion>
    </div>
  );
};
