defmodule Synapses.MixProject do
  use Mix.Project

  def project do
    [
      app: :synapses,
      version: "7.4.1",
      elixir: "~> 1.9",
      start_permanent: Mix.env() == :prod,
      deps: deps(),

      description: "A neural network library for the Elixir language.",
      package: package()
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
      {:gleam_synapses, "~> 0.0.2"}
    ]
  end

  defp package() do
    [
      # This option is only needed when you don't want to use the OTP application name
      # name: "emel",
      # These are the default files included in the package
      # files: ~w(lib priv .formatter.exs mix.exs README* readme* LICENSE*
      #           license* CHANGELOG* changelog* src),
      licenses: ["Apache 2.0"],
      links: %{
        "GitHub" => "https://github.com/mrdimosthenis/Synapses"
      }
    ]
  end
end
