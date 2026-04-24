CREATE SCHEMA app;
SEt search_path TO app;

CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    login TEXT NOT NULL UNIQUE,
    password_hash TEXT NOT NULL
);

CREATE TABLE friendships (
    initiator_id INT NOT NULL,
    receiver_id INT NOT NULL,
    accepted BOOLEAN NOT NULL,
    CONSTRAINT pk_invitations PRIMARY KEY (initiator_id, receiver_id)
);

CREATE TABLE room_presences (
    room_id INT NOT NULL,
    user_id INT NOT NULL,
    CONSTRAINT pk_room_presences PRIMARY KEY (room_id, user_id)
);

CREATE TABLE rooms (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL UNIQUE
);

CREATE TABLE chat_messages (
    id SERIAL PRIMARY KEY,
    room_id INT NOT NULL,
    author_id INT NOT NULL,
    content TEXT NOT NULL
);

CREATE TABLE refresh_token_states (
    token TEXT PRIMARY KEY,
    user_id INT NOT NULL,
    status TEXT NOT NULL
);

/* Friendship */
ALTER TABLE friendships ADD CONSTRAINT fk_friendships_initiator_id
    FOREIGN KEY (initiator_id) REFERENCES users (id)
;

ALTER TABLE friendships ADD CONSTRAINT fk_friendships_receiver_id
    FOREIGN KEY (receiver_id) REFERENCES users (id)
;

ALTER TABLE friendships ADD CONSTRAINT friendships_no_self_friendship
    CHECK (initiator_id != receiver_id)
;

CREATE UNIQUE INDEX unique_friendship ON friendships (
    LEAST(initiator_id, receiver_id), GREATEST(initiator_id, receiver_id)
);

/* Room presences */
ALTER TABLE room_presences ADD CONSTRAINT fk_room_presences_room_id
    FOREIGN KEY (room_id) REFERENCES rooms (id)
;

ALTER TABLE room_presences ADD CONSTRAINT fk_room_presences_user_id
    FOREIGN KEY (user_id) REFERENCES users (id)
;

/* TODO Replace the trigger with cron job cleanup */
CREATE OR REPLACE FUNCTION fn_trg_room_presences_delete()
RETURNS TRIGGER AS $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM room_presences WHERE room_id = OLD.room_id) THEN
        DELETE FROM rooms WHERE id = OLD.room_id;
    END IF;
    RETURN NEW;
END;
$$LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trg_room_presences_delete
ON room_presences
AFTER DELETE
EXECUTE FUNCTION fn_trg_room_presences_delete();

/* Chat messages */
ALTER TABLE chat_messages ADD CONSTRAINT fk_chat_messages_room_id
    FOREIGN KEY (room_id) REFERENCES rooms (id)
    ON DELETE CASCADE
;

ALTER TABLE chat_messages ADD CONSTRAINT fk_chat_messages_author_id
    FOREIGN KEY (author_id) REFERENCES users (id)
;

/* Refresh token states */
ALTER TABLE refresh_token_states ADD CONSTRAINT fk_refresh_token_states_user_id
    FOREIGN KEY (user_id) REFERENCES users (id)
;

ALTER TABLE refresh_token_states ADD CONSTRAINT refresh_token_states_status_check
    CHECK (status IN ('active', 'used', 'revoked'))
;
    

RESET search_path;