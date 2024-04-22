import { cookies } from "next/headers";
import "server-only";

const getServerApiUrl = () => {
	const env = process.env.SERVER_API_URL;
	if (!env || env === "") {
		throw new Error("SERVER_API_URL env is not set");
	}
	return env;
};

const LOGIN_COOKIE_NAME = ".AspNetCore.Cookies";

export const apiFetch = <T>(
	path: string,
	method: "GET" | "POST" | "PUT",
	body?: T,
) => {
	const cook = cookies();
	const cookieValue = cook.get(LOGIN_COOKIE_NAME);
	const loginValue = cookieValue?.value;

	return fetch(`${getServerApiUrl()}${path}`, {
		method: method,
		headers: {
			...(loginValue ? { Cookie: `${LOGIN_COOKIE_NAME}=${cookieValue}` } : {}),
			"Content-Type": "application/json",
		},
		body: JSON.stringify(body),
	});
};

export const apiGET = async <T>(path: string) => {
	const res = await apiFetch(path, "GET");
	const text = await res.text();
	console.log(text);
	const data = JSON.parse(text);
	return data as T;
};

export const apiPOST = async <T, B>(path: string, body: B) => {
	const res = await apiFetch(path, "POST", body);
	const data = await res.json();
	return data as T;
};

export const apiPUT = async <T, B>(path: string, body: B) => {
	const res = await apiFetch(path, "PUT", body);
	const data = await res.json();
	return data as T;
};
