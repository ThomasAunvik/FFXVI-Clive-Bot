"use client";
import axios from "axios";
import { useCallback, useEffect, useState } from "react";

import { toastError } from "@/components/errors/ErrorHandler";
import useIsMounted from "@/components/misc/useIsMounted";
import { ModeratorListForm } from "@/components/moderator/ModeratorListForm";
import { ModeratorSingleForm } from "@/components/moderator/ModeratorSingleForm";
import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from "@/components/ui/accordion";
import type { IModerator } from "@/lib/models/moderator/ModeratorModel";
import { PlusIcon } from "lucide-react";

export const ModeratorList = () => {
  const isMounted = useIsMounted();

  const [moderators, setModerators] = useState<IModerator[]>([]);

  const [openAddNew, setOpenAddNew] = useState(false);

  const fetchModerator = useCallback(async () => {
    try {
      const res = await axios.get("/api/moderator");
      if (res.status === 200) {
        const newModerators = res.data as IModerator[];
        if (isMounted) {
          setModerators(newModerators);
        }
      }
    } catch (err) {
      toastError(err);
    }
  }, [isMounted]);

  useEffect(() => {
    fetchModerator();
  }, [fetchModerator]);

  return (
    <div className="mb-4">
      <Accordion type="multiple" className="md:max-w-[350px]">
        <AccordionItem value="create-form">
          <AccordionTrigger>
            <PlusIcon />
            Add Moderator
          </AccordionTrigger>
          <AccordionContent>
            <ModeratorSingleForm
              onSuccess={(newMods) => {
                setModerators(newMods);
              }}
              close={() => setOpenAddNew(false)}
            />
          </AccordionContent>
        </AccordionItem>
        {moderators.map((s, i) => {
          return (
            <AccordionItem value={s.id.toString()} key={`moderator-${s.id}`}>
              <AccordionTrigger>
                {s.name} ({s.connectionSource})
              </AccordionTrigger>
              <AccordionContent>
                <ModeratorListForm
                  moderator={s}
                  onDelete={async () => {
                    try {
                      const res = await axios.delete(`/api/moderator/${s.id}`);
                      if (res.status === 200) {
                        setModerators(res.data as IModerator[]);
                      }
                    } catch (err) {
                      toastError(err);
                    }
                  }}
                  onUpdate={(mods) => {
                    setModerators(mods);
                  }}
                />
              </AccordionContent>
            </AccordionItem>
          );
        })}
      </Accordion>
    </div>
  );
};
