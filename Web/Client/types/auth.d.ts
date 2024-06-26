import { Session } from "next-auth";

declare module "next-auth" {
  /**
   * Returned by `useSession`, `getSession` and received as a prop on the `Provider` React Context
   */
  interface Session {
    accessToken: string;
  }
}

declare module "@auth/core" {
  interface JWT {
    accessToken: string;
  }
}
