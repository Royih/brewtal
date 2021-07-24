import React, { useState } from "react";
import { Container } from "@material-ui/core";
import Alert from "@material-ui/lab/Alert";

export type ApiContextState = {
  get<T>(url: string): Promise<T>;
  post<T>(url: string, payload?: any): Promise<T>;
  logout(): void;
};

export interface ILoginResult {
  access_token: string;
  refresh_token: string;
  espires_in: number;
}

const ApiContext = React.createContext({} as ApiContextState);

export { ApiContext };

const BaseApiPath = process.env.REACT_APP_API_PATH + "/api";
const RefreshAccessTokenUrl = "jwt/RefreshAccessToken";
const AccessTokenLocalStorageKey = "access_token";
const RefreshTokenLocalStorageKey = "refresh_token";

export const ApiContextProvider = (props: any) => {
  //const [lastCall, setLastCall] = useState<string>("N/A");
  const [exception, setException] = useState("");
  const [exceptionDetails, setExceptionDetails] = useState("");

  const request = async <T extends unknown>(url: string, method: string, payload?: any): Promise<T | undefined> => {
    let accessToken = localStorage.getItem(AccessTokenLocalStorageKey);

    let headers = new Headers();
    headers.append("Content-Type", "application/json");
    if (accessToken) {
      headers.append("Authorization", `Bearer ${accessToken}`);
    }
    console.debug(`%chttp-${method}: ${url}`, "background: #222; color: #bada55");
    var response = await fetch(BaseApiPath + "/" + url, {
      method: method,
      headers: headers,
      credentials: "include",
      body: JSON.stringify(payload),
    }).catch((error) => {
      console.error("Exception during fetch", error);
      throw error;
    });
    if (response.ok) {
      return response.json();
    } else if (response.status === 401 && url !== RefreshAccessTokenUrl) {
      let refreshAccessTokenOk = await refreshAccessToken();
      if (refreshAccessTokenOk) {
        return await request(url, method, payload);
      }
    } else {
      setException(response.statusText + ": " + url);
      response.text().then(function (text) {
        setExceptionDetails(text);
      });
    }
    console.error("Error calling API: ", response.status);

    //throw new Error(response.statusText);
  };

  const get = async <T extends unknown>(url: string): Promise<T | undefined> => {
    //setLastCall(url);
    return await request<T>(url, "GET");
  };

  const post = async <T extends unknown>(url: string, payload?: any): Promise<T | undefined> => {
    return await request(url, "POST", payload);
  };

  const refreshAccessToken = async (): Promise<boolean> => {
    let refreshToken = localStorage.getItem(RefreshTokenLocalStorageKey);
    if (refreshToken) {
      let payload = {
        refreshToken: refreshToken,
      };
      var response = await post<ILoginResult>(RefreshAccessTokenUrl, payload);
      if (response) {
        console.debug("Got new access token.");
        localStorage.setItem(AccessTokenLocalStorageKey, response.access_token);
        return true;
      }
    }
    return false;
  };

  const contextState = {
    get: get,
    post: post,
  } as ApiContextState;

  const resetException = () => {
    setException("");
  };

  const DisplayException = () => {
    if (exception) {
      return (
        <Container>
          <Alert severity="error" onClick={resetException}>
            <h3>{exception}</h3>
            <em>{exceptionDetails}</em>
          </Alert>
        </Container>
      );
    }
    return null;
  };

  return (
    <ApiContext.Provider value={contextState}>
      {props.children}
      {/* <Container>Last call: {lastCall}</Container> */}
      <DisplayException></DisplayException>
    </ApiContext.Provider>
  );
};
