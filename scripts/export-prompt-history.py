#!/usr/bin/env python3
"""Export a Claude Code session transcript to PromptHistory/<date>-<session>.md.

Run as a Claude Code Stop hook (reads hook JSON on stdin with `transcript_path`
and `session_id`), or manually: export-prompt-history.py <transcript.jsonl>.

Only the developer's prompts and the AI's text replies are exported. Tool calls,
tool results, system context and IDE notices are left out.
"""
import json
import os
import re
import sys
from datetime import datetime
from pathlib import Path

SKIP_PREFIXES = ("<ide_", "<system-reminder", "<command-", "<local-command")


def texts(content):
    blocks = [{"type": "text", "text": content}] if isinstance(content, str) else (content or [])
    out = []
    for b in blocks:
        if isinstance(b, dict) and b.get("type") == "text":
            t = b["text"].strip()
            if t and not t.startswith(SKIP_PREFIXES):
                out.append(t)
    return out


def export(transcript: Path, project_dir: Path) -> Path | None:
    entries = []
    started = None
    for line in transcript.read_text(encoding="utf-8").splitlines():
        try:
            o = json.loads(line)
        except json.JSONDecodeError:
            continue
        if o.get("isSidechain") or o.get("isMeta"):
            continue
        t = o.get("type")
        if t not in ("user", "assistant"):
            continue
        body = "\n\n".join(texts((o.get("message") or {}).get("content")))
        if not body:
            continue
        if started is None and o.get("timestamp"):
            started = o["timestamp"]
        entries.append((t, body))

    if not any(t == "user" for t, _ in entries):
        return None

    date = (started or datetime.now().isoformat())[:10]
    session = transcript.stem[:8]
    out = [
        f"# Prompt-historik – {date} (session {session})\n",
        "Export av en Claude Code-session: utvecklarens prompter och AI:ns svar "
        "(verktygsanrop och systemkontext är utelämnade). Genereras automatiskt av "
        "`scripts/export-prompt-history.py`. Länkar i svaren speglar filstrukturen "
        "vid tillfället och kan vara inaktuella.\n",
    ]
    n = 0
    for t, body in entries:
        if t == "user":
            n += 1
            out.append(f"## {n}. Prompt\n\n> " + body.replace("\n", "\n> ") + "\n\n### Svar\n")
        else:
            # Demote the reply's own headings below the prompt/answer headings.
            out.append(re.sub(r"(?m)^(#{1,4}) ", lambda m: "#" * min(len(m.group(1)) + 2, 6) + " ", body) + "\n")

    dest_dir = project_dir / "PromptHistory"
    dest_dir.mkdir(exist_ok=True)
    dest = dest_dir / f"{date}-{session}.md"
    dest.write_text("\n".join(out), encoding="utf-8")
    return dest


def main():
    project_dir = Path(os.environ.get("CLAUDE_PROJECT_DIR") or Path(__file__).resolve().parent.parent)
    if len(sys.argv) > 1:
        transcript = Path(sys.argv[1])
    else:
        payload = json.load(sys.stdin)
        transcript = Path(payload["transcript_path"])
    if transcript.exists():
        export(transcript, project_dir)


if __name__ == "__main__":
    main()
