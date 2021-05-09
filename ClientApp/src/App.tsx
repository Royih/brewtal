import React from "react";
import "./App.css";
import { ThemeContextProvider } from "./infrastructure/ThemeContextProvider";
import { CssBaseline } from "@material-ui/core";
import { Counter } from "./components/functionDemo/Counter";
import { SnackbarProvider } from "notistack";
import { Layout } from "./components/Layout";
import { Home } from "./components/home/Home";
import { Route } from "react-router";
import { Footer } from "./components/Footer";
import { ApiContextProvider } from "./infrastructure/ApiContextProvider";
import { FetchData } from "./components/functionDemo/FetchData";
import { ThrowExceptions } from "./components/functionDemo/TestExceptions";
import { SignalrContextProvider } from "./infrastructure/SignalrContextProvider";
import { TestSignalR } from "./components/functionDemo/TestSignalR";
import { Brews } from "./components/brews/Brews";
import { SignalrHubContextProvider } from "./infrastructure/SignalrHubContextProvider";
import { EditBrew } from "./components/brews/EditBrew";

function App() {
  return (
    <ApiContextProvider>
        <SignalrHubContextProvider>
          <SignalrContextProvider>
            <ThemeContextProvider>
              <CssBaseline />
              <SnackbarProvider maxSnack={10}>
                <Layout>
                  <Route exact path="/" component={Home} />
                  <Route exact path="/counter" component={Counter} />
                  <Route exact path="/fetch-data" component={FetchData} />
                  <Route exact path="/throw-exceptions" component={ThrowExceptions} />
                  <Route path="/test-signalr" component={TestSignalR} />
                  <Route exact path="/brews" component={Brews} />
                  <Route path="/brew/create" component={EditBrew} />
                  <Route path="/brew/edit/:id" component={EditBrew} />
                </Layout>
                <Footer />
              </SnackbarProvider>
            </ThemeContextProvider>
          </SignalrContextProvider>
        </SignalrHubContextProvider>
    </ApiContextProvider>
  );
}

export default App;
