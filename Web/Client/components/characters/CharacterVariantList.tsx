import { CharacterVariantForm } from "@/components/characters/CharacterVariantForm";
import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from "@/components/ui/accordion";
import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import { Collapsible, CollapsibleContent } from "@/components/ui/collapsible";
import type { ICharacter } from "@/lib/models/characters/CharacterModel";
import { PlusIcon } from "lucide-react";
import { useState } from "react";

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
          <PlusIcon />
        </Button>
      </h2>
      <Collapsible open={openNew}>
        <CollapsibleContent>
          <Card>
            <CardContent>
              <CharacterVariantForm
                onUpdated={newVariant}
                character={character}
                onCancel={() => setOpenNew(false)}
              />
            </CardContent>
          </Card>
        </CollapsibleContent>
      </Collapsible>

      <Accordion type="multiple">
        {variants.map((v) => {
          return (
            <AccordionItem key={`variant-${v.id}`} value={`variant-${v.id}`}>
              <AccordionTrigger>
                <div>
                  <div className="flex flex-row">
                    <span>Age: {v.age}</span>
                    <span>
                      Year {v.fromYear} - {v.toYear}
                    </span>
                  </div>
                </div>
              </AccordionTrigger>
              <AccordionContent>
                <CharacterVariantForm
                  variant={v}
                  onUpdated={updateVariant}
                  character={character}
                />
              </AccordionContent>
            </AccordionItem>
          );
        })}
      </Accordion>
    </div>
  );
};
