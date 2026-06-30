import subprocess
from pathlib import Path
from .runtime import RUNTIME_DIR


def run_command(cmd, quiet: bool = False) -> subprocess.CompletedProcess:
    cmd = [str(x) for x in cmd]
    result = subprocess.run(
        cmd,
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        text=True,
        cwd=str(RUNTIME_DIR),
    )
    if result.returncode != 0 and not quiet:
        print()
        print("Command failed:")
        print(" ".join(cmd))
        if result.stdout.strip():
            print("\nSTDOUT:")
            print(result.stdout.strip())
        if result.stderr.strip():
            print("\nSTDERR:")
            print(result.stderr.strip())
    return result
