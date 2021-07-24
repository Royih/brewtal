
import { Container } from "@material-ui/core";

export const Layout = (props: any) => {
    return (
        <div>
            <Container>{props.children}</Container>
        </div>
    );
};
