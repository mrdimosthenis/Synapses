defmodule Synapses.MixProject do
  use Mix.Project

  def project do
    [
      app: :synapses,
      version: "0.1.0",
      elixir: "~> 1.9",
      start_permanent: Mix.env() == :prod,
      deps: deps()
    ]
  end

  # Run "mix help compile.app" to learn about applications.
  def application do
    [
      extra_applications: [:logger]
    ]
  end

  # Run "mix help deps" to learn about dependencies.
  defp deps do
    [
      # {:dep_from_hexpm, "~> 0.3.0"},
      # {:dep_from_git, git: "https://github.com/elixir-lang/my_dep.git", tag: "0.1.0"}
      {:gleam_stdlib, "~> 0.14.0"},
      {:gleam_decode, "~> 1.7.0", override: true},
      {:gleam_synapses, "~> 0.0.1"},
      {:csv, "~> 2.4.1", only: [:test]}
    ]
  end
end
