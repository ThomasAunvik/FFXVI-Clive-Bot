import { NoAuthError } from "@/lib/errors";
import type { UserModel } from "../models/user/UserModel";
import { apiGET } from "./fetch";

export const getCurrentUser = async () => {
  try {
    return await apiGET<UserModel>("/api/user/current");
  } catch (err) {
    if (err instanceof NoAuthError) {
      return null;
    }
    throw err;
  }
};
