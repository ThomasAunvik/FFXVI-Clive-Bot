import DiscordIcon from "@/components/icons/DiscordIcon";
import { ThemeToggle } from "@/components/misc/ThemeToggle";
import { Button } from "@/components/ui/button";
import { cn } from "@/lib/utils";
import "@/styles/home.css";
import Image from "next/image";
import Link from "next/link";

const HomePage = () => {
  return (
    <div>
      <div>
        {/* <MainPageNavBar currentPath="/" /> */}

        <div
          className={cn(
            "h-full min-h-96 w-full",
            "bg-[url('/static/images/clive/clive_bg.webp')] bg-center bg-cover bg-no-repeat lg:bg-[center_top_-4rem]",
          )}
        >
          <div
            className={"flex h-[60vh] min-h-96 w-full flex-col justify-center"}
          >
            <p className={"flex flex-row justify-center gap-2"}>
              <span className={"titleff pt-2"}>FFXVI</span>
              <span className="self-start text-3xl">Clive Bot</span>
            </p>
            <br />
            <div className="flex flex-row justify-center gap-4">
              <Link
                href="https://discord.com/oauth2/authorize?client_id=1070508781013848144&scope=bot&permissions=0"
                target="_blank"
              >
                <Button>
                  <span className="flex flex-row gap-2">
                    <span>Add Bot to Discord </span>
                    <DiscordIcon className="h-6 w-6 fill-black" />
                  </span>
                </Button>
              </Link>
              <br />
              <Link href="https://discord.gg/y34bsEg" target="_blank">
                <Button>
                  <span>Join Discord Server </span>
                </Button>
              </Link>
            </div>
          </div>
        </div>
      </div>
      <div className="flex flex-col gap-4 pt-4 pr-8 pl-8">
        <p className="font-bold text-4xl">Features:</p>

        <ul className="list-disc">
          <li>
            <p className="text-lg">Skill Preview</p>
          </li>
          <Image
            alt="Skill Preview of Lunge attack"
            src="/static/images/features/command_skill_preview.webp"
            width={523}
            height={516}
            className={"imagecontain"}
          />
        </ul>
      </div>
    </div>
  );
};

export default HomePage;
