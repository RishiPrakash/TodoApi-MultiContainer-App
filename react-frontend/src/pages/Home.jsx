import { useMsal, AuthenticatedTemplate, UnauthenticatedTemplate } from '@azure/msal-react';
import { Container } from 'react-bootstrap';
import { PublicEndpoint } from '../components/PublicEndpoint';
import { AuthenticatedEndpoint } from '../components/AuthenticatedEndpoint';

export const Home = () => {
    return (
        <Container>
            <h2 className="text-center mb-4">API Demo</h2>
            
            {/* Public endpoint - always visible */}
            <PublicEndpoint />
            
            {/* Authenticated endpoint - only visible when signed in */}
            <AuthenticatedTemplate>
                <AuthenticatedEndpoint />
            </AuthenticatedTemplate>
            
            {/* Message shown when not signed in */}
            <UnauthenticatedTemplate>
                <div className="alert alert-info mt-3">
                    Please sign in to view the authenticated endpoint data.
                </div>
            </UnauthenticatedTemplate>
        </Container>
    );
};