import { ThemeToggle } from "@/components/misc/ThemeToggle";
import { Button } from "@/components/ui/button";
import "@/styles/home.css";
import Image from "next/image";
import Link from "next/link";

const HomePage = () => {
  return (
    <div>
      <div>
        {/* <MainPageNavBar currentPath="/" /> */}
        <ThemeToggle />
        <div className={"conten"}>
          <div className={"child"}>
            <p className={"title"}>
              <span className={"titleff"}>FFXVI</span> Clive Bot
            </p>
            <br />
            <Link
              href="https://discord.com/oauth2/authorize?client_id=1070508781013848144&scope=bot&permissions=0"
              target="_blank"
            >
              <Button>
                <span>
                  Add Bot to Discord{" "}
                  <Image
                    alt="discord-icon"
                    src="/static/images/discord/discord-mark-white.svg"
                    width={20}
                    height={20}
                    className={"pull-right"}
                  />
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
      <div className={"features"}>
        <h1>Features</h1>

        <h2>Skill Preview</h2>
        <Image
          alt="Skill Preview of Lunge attack"
          src="/static/images/features/command_skill_preview.webp"
          width={523}
          height={516}
          className={"imagecontain"}
        />
      </div>
    </div>
  );
};

export default HomePage;
