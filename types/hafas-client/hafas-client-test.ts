import createClient from 'hafas-client';
import dbProfile from 'hafas-client/p/db';

const client: HafasClient = createClient(dbProfile, 'my-awesome-program');
