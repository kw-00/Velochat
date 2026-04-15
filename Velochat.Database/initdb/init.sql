CREATE SCHEMA app;
SEt search_path TO app;

CREATE TABLE identities (
    id SERIAL PRIMARY KEY,
    login TEXT NOT NULL UNIQUE,
    password_hash TEXT NOT NULL
);

CREATE TABLE room_presences (
    room_id INT NOT NULL,
    member_id INT NOT NULL,
    CONSTRAINT pk_room_presences PRIMARY KEY (room_id, member_id)
);

CREATE TABLE invitations (
    room_id INT NOT NULL,
    invitee_id INT NOT NULL,
    CONSTRAINT pk_invitations PRIMARY KEY (room_id, invitee_id)
);

CREATE TABLE rooms (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL UNIQUE,
    owner_id INT NOT NULL
);

CREATE TABLE chat_messages (
    id SERIAL PRIMARY KEY,
    room_id INT NOT NULL,
    author_id INT NOT NULL,
    content TEXT NOT NULL
);

CREATE TABLE refresh_token_states (
    token TEXT PRIMARY KEY,
    identity_id INT NOT NULL,
    status TEXT NOT NULL
);

ALTER TABLE room_presences ADD CONSTRAINT fk_room_presences_room_id
    FOREIGN KEY (room_id) REFERENCES rooms (id)
;

ALTER TABLE room_presences ADD CONSTRAINT fk_room_presences_member_id
    FOREIGN KEY (member_id) REFERENCES identities (id)
;

ALTER TABLE invitations ADD CONSTRAINT fk_invitations_room_id
    FOREIGN KEY (room_id) REFERENCES rooms (id)
;

ALTER TABLE invitations ADD CONSTRAINT fk_invitations_identity_id
    FOREIGN KEY (invitee_id) REFERENCES identities (id)
;

ALTER TABLE rooms ADD CONSTRAINT fk_rooms_owner_id
    FOREIGN KEY (owner_id) REFERENCES identities (id)
;

ALTER TABLE rooms ADD CONSTRAINT unique_owner_name UNIQUE (owner_id, name);

ALTER TABLE chat_messages ADD CONSTRAINT fk_chat_messages_room_id
    FOREIGN KEY (room_id) REFERENCES rooms (id)
;

ALTER TABLE chat_messages ADD CONSTRAINT fk_chat_messages_author_id
    FOREIGN KEY (author_id) REFERENCES identities (id)
;

ALTER TABLE refresh_token_states ADD CONSTRAINT fk_refresh_token_states_identity_id
    FOREIGN KEY (identity_id) REFERENCES identities (id)
;

ALTER TABLE refresh_token_states ADD CONSTRAINT fk_refresh_token_states_status_check
    CHECK (status IN ('active', 'used', 'revoked'))
;
    

RESET search_path;