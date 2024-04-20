import { policy } from "@/components/legal/redditapp/toc";
import type { Metadata } from "next";
import React from "react";

export const metadata: Metadata = {
  title: "FFXVI Clive Reddit App - Terms of Conditions",
  description: "FFXVI Clive Reddit App - Terms of Conditions",
};

const DashboardPage = () => {
  const [firstLine, ...rest] = policy.split("\n");

  return (
    <div>
      <p>
        {firstLine}
        {rest.map((line, i) => (
          // React.Fragment doesn’t create a wrapper element in the DOM.
          // If you don’t care about that, you can use div instead
          // biome-ignore lint/suspicious/noArrayIndexKey: <explanation>
          <React.Fragment key={`line-${i}`}>
            <br />
            {line}
          </React.Fragment>
        ))}
      </p>
    </div>
  );
};

export default DashboardPage;
