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
import { UserContextProvider } from "./infrastructure/UserContextProvider";
import { FetchData } from "./components/functionDemo/FetchData";
import { Users } from "./components/users/Users";
import { EditUserProfile } from "./components/users/EditUserProfile";
import { ThrowExceptions } from "./components/functionDemo/TestExceptions";
import { SignalrContextProvider } from "./infrastructure/SignalrContextProvider";
import { TestSignalR } from "./components/functionDemo/TestSignalR";
import { Brews } from "./components/brews/Brews";
import { SignalrHubContextProvider } from "./infrastructure/SignalrHubContextProvider";

function App() {
    return (
        <ApiContextProvider>
            <UserContextProvider>
                <SignalrHubContextProvider>
                    <SignalrContextProvider>
                        <ThemeContextProvider>
                            <CssBaseline />
                            <SnackbarProvider maxSnack={10}>
                                <div>
                                    <Layout>
                                        <Route exact path="/" component={Home} />
                                        <Route exact path="/counter" component={Counter} />
                                        <Route exact path="/fetch-data" component={FetchData} />
                                        <Route exact path="/throw-exceptions" component={ThrowExceptions} />
                                        <Route exact path="/users" component={Users} />
                                        <Route path="/user/create" component={EditUserProfile} />
                                        <Route path="/users/:id" component={EditUserProfile} />
                                        <Route path="/test-signalr" component={TestSignalR} />
                                        <Route exact path="/brews" component={Brews} />
                                    </Layout>
                                    <Footer />
                                </div>
                            </SnackbarProvider>
                        </ThemeContextProvider>
                    </SignalrContextProvider>
                </SignalrHubContextProvider>
            </UserContextProvider>
        </ApiContextProvider>
    );
}

export default App;
