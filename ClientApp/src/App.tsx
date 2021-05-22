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
import { SignalrContextProvider } from "./infrastructure/SignalrContextProvider"

function App() {
  return (
    <ApiContextProvider>
      <SignalrContextProvider>
        <ThemeContextProvider>
          <CssBaseline />
          <SnackbarProvider maxSnack={10}>
            <Layout>
              <Route exact path="/" component={Home} />
              <Route exact path="/counter" component={Counter} />
              <Route exact path="/fetch-data" component={FetchData} />
              <Route exact path="/throw-exceptions" component={ThrowExceptions} />
            </Layout>
            <Footer />
          </SnackbarProvider>
        </ThemeContextProvider>
      </SignalrContextProvider>
    </ApiContextProvider>
  );
}

export default App;
